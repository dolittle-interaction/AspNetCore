// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Dolittle.Events;
using Dolittle.Events.Handling;
using MongoDB.Driver;

namespace Debugging.MyFeature
{
    /// <summary>
    /// Example EventHandler.
    /// </summary>
    [EventHandler("1964b0ef-213e-4ac7-8498-c6b9ec37554a")]
    public class MyEventHandler : ICanHandleEvents
    {
        readonly IMongoCollection<MyReadModel> _collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="MyEventHandler"/> class.
        /// </summary>
        /// <param name="collection">MongoCollection for the readmodel.</param>
        public MyEventHandler(IMongoCollection<MyReadModel> collection)
        {
            _collection = collection;
        }

        /// <summary>
        /// Example Handle method.
        /// </summary>
        /// <param name="event">Event to be handled.</param>
        /// <param name="eventContext">EventContext.</param>
        /// <returns>Task.</returns>
        public Task Handle(MyEvent @event, EventContext eventContext)
        {
            _collection.InsertOne(new MyReadModel
            {
                MyString = @event.MyString
            });
            System.Console.WriteLine(@event);
            System.Console.WriteLine(eventContext);
            return Task.CompletedTask;
        }
    }
}
