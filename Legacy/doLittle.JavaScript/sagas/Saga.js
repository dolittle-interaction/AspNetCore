Dolittle.namespace("Dolittle.sagas");
Dolittle.sagas.Saga = (function () {
    function Saga() {
        var self = this;

        this.executeCommands = function (commands, options) {

            var canExecuteSaga = true;

            commands.forEach(function (command) {
                if (command.onBeforeExecute() === false) {
                    canExecuteSaga = false;
                    return false;
                }
            });

            if (canExecuteSaga === false) {
                return;
            }
            Dolittle.commands.commandCoordinator.handleForSaga(self, commands, options);
        };
    }

    return {
        create: function (configuration) {
            var saga = new Saga();
            Dolittle.extend(saga, configuration);
            return saga;
        }
    };
})();
