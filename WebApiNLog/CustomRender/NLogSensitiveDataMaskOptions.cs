namespace WebApiNLog.CustomRender
{
    public class NLogSensitiveDataMaskOptions
    {
        public IDictionary<string, Func<string, string>> MaskDataDic { get; set; }
    }
}
