﻿namespace CodeNet.Parameters.Settings;

public class ParameterSettings
{
    private string? _redisPrefix;

    /// <summary>
    /// Redis Prefix Key
    /// </summary>
    public string RedisPrefix
    {
        get
        {
            return string.IsNullOrEmpty(_redisPrefix) ? "CNPRM" : (_redisPrefix ?? string.Empty);
        }
        set { _redisPrefix = value; }
    }

    private int? _time;

    /// <summary>
    /// Minutes
    /// </summary>
    public int Time
    {
        get
        {
            return _time ?? 360;
        }
        set { _time = value; }
    }

    private bool? _useRedis;

    /// <summary>
    /// Should Redis be used?
    /// </summary>
    public bool UseRedis
    {
        get
        {
            return _useRedis ?? true;
        }
        set { _useRedis = value; }
    }
}
