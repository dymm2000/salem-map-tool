using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SalemMapTool.MergeWizard
{
    public delegate void PartFinishedEventHandler(Session leftSession, Session rightSession, Session resultSession);
    public partial class MergeWizardPart : UserControl
    {
        private class TileMatch
        {
            public Point LeftPoint { get; set; }
            public Point RightPoint { get; set; }
        }

        private Session _leftSession;
        private Session _rightSession;
        private List<TileMatch> _matches;
        private int _currentMatchIndex;

        public event PartFinishedEventHandler PartFinished;

        public MergeWizardPart()
        {
            InitializeComponent();
        }

        public void LoadSessions(Session leftSession, Session rightSession)
        {
            _leftSession = leftSession;
            _rightSession = rightSession;
            
            _matches = new List<TileMatch>();
            var leftHash = _leftSession.GenerateHash();
            var rightHash = rightSession.GenerateHash();

            Session.Tile leftTile;
            Session.Tile rightTile;
            foreach (var tileHash in leftHash.Keys)
            {
                leftTile = leftHash[tileHash];
                rightHash.TryGetValue(tileHash, out rightTile);

                if (rightTile != null)
                {
                    var match = new TileMatch()
                    {
                        LeftPoint = new Point(leftTile.X, leftTile.Y),
                        RightPoint = new Point(rightTile.X, rightTile.Y)
                    };
                    _matches.Add(match);
                }
            }

            _currentMatchIndex = 0;

            rightSessionPictureBox.UpdateSession(_rightSession);
            leftSessionPictureBox.UpdateSession(_leftSession);

            UpdateMatch();
        }

        private void btnMergeNext_Click(object sender, System.EventArgs e)
        {
            var sessionName = string.Format("Merge {0:yyyy-MM-dd HH.mm.ss}", DateTime.Now);
            var session = new Session(sessionName);

            var sessionsList = new List<Session>();
            sessionsList.Add(_leftSession);
            sessionsList.Add(_rightSession);
			if (session.Load(sessionsList))
			{
                if (PartFinished != null)
                    PartFinished(_leftSession, _rightSession, session);
			}
			else
			{
                btnCancel.PerformClick();
			}
        }

        private void btnSkip_Click(object sender, System.EventArgs e)
        {
            if (PartFinished != null)
                PartFinished(_leftSession, _rightSession, _leftSession);
        }

        private void UpdateMatch()
        {
            var match = _matches[_currentMatchIndex % _matches.Count];

            //_leftSession.Choose(match.LeftPoint.X, match.LeftPoint.Y);
            _rightSession.Choose(match.RightPoint.X, match.RightPoint.Y);

            rightSessionPictureBox.Refresh();
            //leftSessionPictureBox.Refresh();
        }

        private void btnNextMatch_Click(object sender, EventArgs e)
        {
            ++_currentMatchIndex;
            UpdateMatch();
        }

        private void btnPrevMatch_Click(object sender, EventArgs e)
        {
            --_currentMatchIndex;
            UpdateMatch();
        }
    }
}
