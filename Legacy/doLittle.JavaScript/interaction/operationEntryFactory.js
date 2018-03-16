Dolittle.namespace("Dolittle.interaction", {
    operationEntryFactory: Dolittle.Singleton(function () {
        /// <summary>Represents a factory that can create OperationEntries</summary>

        this.create = function (operation, state) {
            /// <sumary>Create an instance of a OperationEntry</summary>
            /// <param name="context" type="Dolittle.interaction.OperationContext">Context the operation was performed in</param>
            /// <param name="operation" type="Dolittle.interaction.Operation">Operation that was performed</param>
            /// <param name="state" type="object">State that operation generated</param>
            /// <returns>An OperationEntry</returns>

            var instance = Dolittle.interaction.OperationEntry.create({
                operation: operation,
                state: state
            });
            return instance;
        };
    })
});