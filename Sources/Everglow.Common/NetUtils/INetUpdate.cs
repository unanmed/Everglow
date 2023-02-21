﻿namespace Everglow.Core.NetUtils
{
	public interface INetUpdate<T>
	{
		public void LocalUpdate(T input);
		public void NetUpdate(T input);
		public void Forcast();
	}
}
