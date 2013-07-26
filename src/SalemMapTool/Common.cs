using System;
using System.Collections;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace SalemMapTool
{
    public class Common
    {

        private static Common _instance;
        public static Common Instance
        {
            get { return _instance ?? (_instance = new Common()); }
        }

        private readonly Hashtable parameters;
        private readonly ExportParams exportParams;
        private Common()
        {
            parameters = new Hashtable();
            exportParams = new ExportParams();
            InitParams();
        }

        private void InitParams()
        {
            string userprofile = Environment.GetEnvironmentVariable(Consts.s_userprofile);

            uint uvalue;
            string value = ConfigurationManager.AppSettings[Consts.s_backColor];
            if (!uint.TryParse(value, NumberStyles.HexNumber, null, out uvalue))
                uvalue = 0x4040ff;
            parameters[Consts.s_backColor] = Color.FromArgb((int)(0xff000000 | uvalue));

            value = ConfigurationManager.AppSettings[Consts.s_importMinSize];
            if (!uint.TryParse(value, NumberStyles.Integer, null, out uvalue))
                uvalue = 0;
            parameters[Consts.s_importMinSize] = Math.Max(0, uvalue);

            value = ConfigurationManager.AppSettings[Consts.s_importDir];
            parameters[Consts.s_importDir] = value == null ? userprofile : value.ToLower().Replace(string.Format("%{0}%", Consts.s_userprofile), userprofile);

            value = ConfigurationManager.AppSettings[Consts.s_exportDir];
            parameters[Consts.s_exportDir] = value == null ? userprofile : value.ToLower().Replace(string.Format("%{0}%", Consts.s_userprofile), userprofile);

            
        }


        public Hashtable Parameters
        {
            get { return parameters; }
        }

        public ExportParams ExportParams
        {
            get { return exportParams; }
        }

        public void ReadParameters(string[] args)
        {
            var userprofile = Environment.GetEnvironmentVariable(Consts.s_userprofile);
            foreach (string[] s in args.Select(a => a.ToLower().
                                                      Replace("'", "").
                                                      Replace("\"", "").
                                                      Replace(string.Format("%{0}%", Consts.s_userprofile), userprofile).
                                                      Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries)).
                                                      Where(s => s.Length == 2))
            {
                uint uvalue;
                switch (s[0])
                {
                    case Consts.s_backColor:
                        if (uint.TryParse(s[1], NumberStyles.HexNumber, null, out uvalue))
                            parameters[Consts.s_backColor] = Color.FromArgb((int)(0xff000000 | uvalue));
                        break;
                    case Consts.s_importDir:
                        parameters[Consts.s_importDir] = s[1];
                        break;
                    case Consts.s_exportDir:
                        parameters[Consts.s_exportDir] = s[1];
                        break;
                    case Consts.s_importMinSize:
                        uvalue = (uint) parameters[Consts.s_importMinSize];
                        parameters[Consts.s_importMinSize] = Math.Max(0, uvalue);
                        break;
                }
            }

            exportParams.Directory = (string)parameters[Consts.s_exportDir];
        }
    }

    public static class Consts
    {
        public const string s_backColor = "backcolor";
        public const string s_importMinSize = "importminsize";
        public const string s_importDir = "importdir";
        public const string s_exportDir = "exportdir";
        public const string s_userprofile = "userprofile";
    }
}
