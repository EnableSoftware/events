using Events.Shared.Models;
using System;

namespace Events.Client.State
{
    public class UserState
    {
        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public UserModel UserInfo { get; private set; }

        public UserState()
        {
        }

        public void SetUserInfo(UserModel userInfo)
        {
            UserInfo = userInfo;
            NotifyStateChanged();
        }
    }
}
