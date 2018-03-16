Dolittle.namespace("Dolittle",{
    taskFactory: Dolittle.Singleton(function () {
        this.createHttpPost = function (url, payload) {
            var task = Dolittle.tasks.HttpPostTask.create({
                url: url,
                payload: payload
            });
            return task;
        };

        this.createHttpGet = function (url, payload) {
            var task = Dolittle.tasks.HttpGetTask.create({
                url: url,
                payload: payload
            });
            return task;
        };

        this.createQuery = function (query, paging) {
            var task = Dolittle.read.QueryTask.create({
                query: query,
                paging: paging
            });
            return task;
        };

        this.createReadModel = function (readModelOf, propertyFilters) {
            var task = Dolittle.read.ReadModelTask.create({
                readModelOf: readModelOf,
                propertyFilters: propertyFilters
            });
            return task;
        };

        this.createHandleCommand = function (command) {
            var task = Dolittle.commands.HandleCommandTask.create({
                command: command
            });
            return task;
        };

        this.createHandleCommands = function (commands) {
            var task = Dolittle.commands.HandleCommandsTask.create({
                commands: commands
            });
            return task;
        };

        this.createViewLoad = function (files) {
            var task = Dolittle.views.ViewLoadTask.create({
                files: files
            });
            return task;
        };

        this.createViewModelLoad = function (files) {
            var task = Dolittle.views.ViewModelLoadTask.create({
                files: files
            });
            return task;
        };

        this.createFileLoad = function (files) {
            var task = Dolittle.tasks.FileLoadTask.create({
                files: files
            });
            return task;
        };
    })
});