using NLog.LayoutRenderers;
using NLog.Layouts;
using NLog;
using System.Text;

namespace WebApiNLog.CustomRender
{
    public class SensitiveMaskLayoutRenderer : LayoutRenderer
    {
        public Layout Inner { get; set; }

        private readonly IDictionary<string, Func<string, string>> _maskDataDic;

        private const string SplitRule1 = "\"";
        private const string SplitRule2 = "&";

        public SensitiveMaskLayoutRenderer()
        {
            
        }

        // unused
        public SensitiveMaskLayoutRenderer(NLogSensitiveDataMaskOptions options)
        {
            _maskDataDic = options.MaskDataDic;
        }

        protected override void Append(StringBuilder builder,
            LogEventInfo logEvent)
        {
            var result = Inner?.Render(logEvent);

            if (!string.IsNullOrWhiteSpace(result))
                builder.Append(MaskSensitiveData(result));
        }

        protected string MaskSensitiveData(string rawLog)
        {
            try
            {
                // check the rawLog content matches all the split rules

                // if split rule matched

                // check the split string array has matched the key in the _maskDataDic

                // replace the value (essentially is string manipulation)

                return rawLog + " *masked* ";
            }
            catch (Exception)
            {
                return rawLog;
            }
        }
    }
}
