Dolittle.namespace("Dolittle.commands", {
    HandleCommandTask: Dolittle.tasks.ExecutionTask.extend(function (command, server, systemEvents) {
        /// <summary>Represents a task that can handle a command</summary>
        this.name = command.name;

        this.execute = function () {
            var promise = Dolittle.execution.Promise.create();

            var commandRequest = Dolittle.commands.CommandRequest.createFrom(command);
            var parameters = {
                command: commandRequest
            };

            var url = "/Dolittle/CommandCoordinator/Handle?_cmd=" + encodeURIComponent(command._commandType);

            server.post(url, parameters).continueWith(function (result) {
                var commandResult = Dolittle.commands.CommandResult.createFrom(result);

                if (commandResult.success === true) {
                    systemEvents.commands.succeeded.trigger(result);
                } else {
                    systemEvents.commands.failed.trigger(result);
                }

                promise.signal(commandResult);
            }).onFail(function (response) {
                var commandResult = Dolittle.commands.CommandResult.create();
                commandResult.exception = "HTTP 500";
                commandResult.exceptionMessage = response.statusText;
                commandResult.details = response.responseText;
                systemEvents.commands.failed.trigger(commandResult);
                promise.signal(commandResult);
            });

            return promise;
        };
    })
});