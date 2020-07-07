// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dolittle.Commands.Handling;
using Dolittle.Domain;

namespace Debugging.MyFeature
{
    /// <summary>
    /// Example CommandHandler.
    /// </summary>
    public class MyCommandHandler : ICanHandleCommands
    {
        readonly IAggregateOf<MyAggregateRoot> _myAggregateRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyCommandHandler"/> class.
        /// </summary>
        /// <param name="myAggregateRoot">imagine .</param>h
        public MyCommandHandler(IAggregateOf<MyAggregateRoot> myAggregateRoot)
        {
            _myAggregateRoot = myAggregateRoot;
        }

        /// <summary>
        /// Example Handle method.
        /// </summary>
        /// <param name="command">le comanda.</param>
        public void Handle(MyCommand command)
        {
             _myAggregateRoot
                .Rehydrate(command.CommandId)
                .Perform(_ => _.DoThing(command.CommandStrings));
        }

        /// <summary>
        /// Excample second Handle method.
        /// </summary>
        /// <param name="command">okk.</param>
        public void Handle(MySecondCommand command)
        {
            System.Console.WriteLine($"secommd comand ok {command.GetType()}");
        }
    }
}
