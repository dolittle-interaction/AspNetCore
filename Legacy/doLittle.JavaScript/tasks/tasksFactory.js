Dolittle.namespace("Dolittle.tasks", {
    tasksFactory: Dolittle.Singleton(function () {
        this.create = function () {
            var tasks = Dolittle.tasks.Tasks.create();
            return tasks;
        };
    })
});
Dolittle.WellKnownTypesDependencyResolver.types.tasksFactory = Dolittle.tasks.tasksFactory;