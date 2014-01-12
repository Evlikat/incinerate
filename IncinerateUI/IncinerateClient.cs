using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;
using IncinerateService.API;

namespace IncinerateUI
{
    class IncinerateClient
    {
        public CommandResult Execute(Command command)
        {
            try
            {
                using (ChannelFactory<IIncinerateService> clientFactory = new ChannelFactory<IIncinerateService>())
                {
                    NamedPipeTransportBindingElement transport = new NamedPipeTransportBindingElement();
                    CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), transport);
                    clientFactory.Endpoint.Binding = binding;
                    clientFactory.Endpoint.Address = new EndpointAddress("net.pipe://IncinerateService/incinerate");
                    IIncinerateService incinerateProxy = clientFactory.CreateChannel();
                    CommandResult result = command.Execute(incinerateProxy);
                    clientFactory.Close();
                    return result;
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Illegal agruments");
            }
            catch (CommunicationObjectFaultedException ex)
            {
                throw new ServiceException("Service is not working", ex);
            }
            catch (EndpointNotFoundException ex)
            {
                throw new ServiceException("Service is not running", ex);
            }
            catch (CommunicationException ex)
            {
                throw new ServiceException("Can not connect to service", ex);
            }
            return null;
        }
    }

    public class ServiceException : Exception
    {
        public ServiceException(String message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
