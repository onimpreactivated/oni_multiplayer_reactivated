using MultiplayerMod.Core.Logging;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace MultiplayerMod.Platform.Lan.Network;

public class LanConfiguration
{
    public static LanConfiguration instance = new LanConfiguration();
    public static void reload() {
        var newConfig = new LanConfiguration();
        if (newConfig.isConfigured != instance.isConfigured) {
            instance.log.Warning("Unable to enable/disable lan configuration while running, please restart Oxygen Not Included.");
            return;
        }
        instance = newConfig;
    }

    public bool isConfigured { get { return configured; } }
    public string serverIp {  get { return hostip; } }
    public ushort serverPort {  get { return hostport; } }

    public string hostUrl { get {
            return "ws://" + serverIp + ":" + serverPort;
        }
    }

    public string displayName {  get { return serverIp + " (" + serverPort + ")"; } }
    public string playerName { get { return name; } }


    private string hostfilename = "lanconfig.txt";
    private bool configured = false;
    private string name = "LanPlayer";
    private string hostip = "127.0.0.1";
    private ushort hostport = 7171;
    private readonly Core.Logging.Logger log = LoggerFactory.GetLogger<LanConfiguration>();

    private LanConfiguration() {
        //log.Level = LogLevel.Debug;
        configured = readConfiguration();
    }

    private bool readConfiguration() {
        string moddir = Path.GetDirectoryName(Assembly.GetAssembly(typeof(LanConfiguration)).Location);
        string fulldir = Path.Combine(moddir, hostfilename);
        if (!File.Exists(fulldir)) {
            return false;
        }

        try {
            using (FileStream fileStream = File.Open(fulldir, FileMode.Open, FileAccess.Read, FileShare.None)) {
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, 1024)) {
                    bool foundHost = false;
                    string line;
                    while ((line = streamReader.ReadLine()) != null) {
                        if (line.IndexOf("#") > -1) { line = line.Substring(0, line.IndexOf("#")); }
                        int splitat = line.IndexOf('=');
                        if (splitat == -1) { continue; }

                        string key = line.Substring(0, splitat).Trim().ToLowerInvariant();
                        string value = line.Substring(splitat + 1).Trim();
                        switch (key) {
                            case "hostip":
                                hostip = value;
                                foundHost = true;
                                continue;
                            case "hostport":
                                hostport = ushort.Parse(value);
                                continue;
                            case "playername":
                                name = value;
                                continue;
                        }
                    }
                    return foundHost;
                }
            }
        } catch (Exception e) {
            log.Warning("Unable to read lan configuration from " + fulldir + ": "+e.Message);
        }
        return false;
    }

}
