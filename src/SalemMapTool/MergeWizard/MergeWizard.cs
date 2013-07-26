using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SalemMapTool.MergeWizard
{
    public partial class MergeWizard : Form
    {
        public class MergeSession
        {
            public Session Session { get; set; }
            public bool Skipped { get; set; }

            public MergeSession()
            {
                Skipped = false;
            }
            public MergeSession(Session session) : this()
            {
                Session = session;
            }
        }

        private readonly Session _originalSession;
        private readonly List<MergeSession> _matchinSessionses;
        private int _currentMatchIndex;

        private Session _finalSession;

        public MergeWizard(Session originalSession, List<Session> matchinSessions)
        {
            InitializeComponent();

            _matchinSessionses = matchinSessions.Select(session => new MergeSession(session)).ToList();

            mwPartStage.PartFinished += (session, rightSession, resultSession) =>
                {
                    if (session == resultSession)
                        _matchinSessionses[_currentMatchIndex].Skipped = true;

                    if (++_currentMatchIndex != matchinSessions.Count)
                        mwPartStage.LoadSessions(resultSession, _matchinSessionses[_currentMatchIndex].Session);
                    else
                    {
                        mwFinishStage.LoadSession(resultSession);
                        mwFinishStage.Visible = true;

                        mwPartStage.Visible = false;
                    }
                };
    
            _originalSession = originalSession;
        }

        public DialogResult StartWizard()
        {
            _finalSession = null;
            mwFinishStage.Visible = false;
            _currentMatchIndex = 0;
            
            mwPartStage.LoadSessions(_originalSession, _matchinSessionses[_currentMatchIndex].Session);
            mwPartStage.Visible = true;

            DialogResult dialogResult = ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                _finalSession = mwFinishStage.FinalSession;
            }

            return dialogResult;
        }

        public Session FinalSession
        {
            get { return _finalSession; }
        }

        public List<MergeSession> MatchingSessions
        {
            get { return _matchinSessionses; }
        }
    }
}
