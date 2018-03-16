Dolittle.namespace("Dolittle.tasks", {
    LoadTask: Dolittle.tasks.Task.extend(function () {
        /// <summary>Represents a base task that represents anything that is loading things</summary>
        this.execute = function () {
            var promise = Dolittle.execution.Promise.create();
            promise.signal();
            return promise;
        };
    })
});