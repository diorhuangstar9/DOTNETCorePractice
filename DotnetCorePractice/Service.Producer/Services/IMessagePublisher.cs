using System;
using System.Threading.Tasks;

namespace Service.Producer.Services
{
	public interface IMessagePublisher
	{

		public Task Publish<T>(T obj);
	}
}

