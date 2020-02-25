using System;

public class Region
{
    public CloudRegionCode Code;
    public string HostAndPort;
    public int Ping;

    public static CloudRegionCode Parse(string codeAsString)
    {
        codeAsString = codeAsString.ToLower();
        CloudRegionCode none = CloudRegionCode.none;
        if (Enum.IsDefined(typeof(CloudRegionCode), codeAsString))
        {
            none = (CloudRegionCode) ((int) Enum.Parse(typeof(CloudRegionCode), codeAsString));
        }
        return none;
    }

    public override string ToString()
    {
        return string.Format("'{0}' \t{1}ms \t{2}", this.Code, this.Ping, this.HostAndPort);
    }
}

