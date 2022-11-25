namespace ReaderAndWriterLockSlim
{
    public class SyncCache
    {
        private readonly ReaderWriterLockSlim _cacheLock = new ReaderWriterLockSlim();
        private readonly Dictionary<int, string> _innerCache = new Dictionary<int, string>();

        public int Count
        {
            get { return _innerCache.Count; }
        }

        public string Read(int id) 
        {
            _cacheLock.EnterReadLock();
            try
            {
                return _innerCache[id];
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }

        public void Add(int id, string value)
        {
            _cacheLock.EnterWriteLock();
            try
            {
                _innerCache.Add(id, value);
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }

        public AddOrUpdateStatus AddOrUpdate(int id, string value)
        {
            _cacheLock.EnterUpgradeableReadLock();
            try
            {
                string result = null;
                if (_innerCache.TryGetValue(id, out result))
                {
                    if (result == value)
                    {
                        return AddOrUpdateStatus.Unchanged;
                    }
                    else
                    {
                        _cacheLock.EnterWriteLock();
                        try
                        {
                            _innerCache[id] = value;
                        }
                        finally
                        {
                            _cacheLock.ExitWriteLock();
                        }

                        return AddOrUpdateStatus.Updated;
                    }
                }
                else
                {
                    _cacheLock.EnterWriteLock();
                    try
                    {
                        _innerCache.Add(id, value);
                    }
                    finally
                    {
                        _cacheLock.ExitWriteLock();
                    }

                    return AddOrUpdateStatus.Added;
                }
            }
            finally
            {
                _cacheLock.ExitUpgradeableReadLock();
            }
        }

        ~SyncCache()
        {
            if(_cacheLock != null)
            {
                _cacheLock.Dispose();
            }
        }
    }
}
