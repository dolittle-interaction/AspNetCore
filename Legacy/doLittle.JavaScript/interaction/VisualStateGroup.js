Dolittle.namespace("Dolittle.interaction", {
    VisualStateGroup: Dolittle.Type.extend(function (dispatcher) {
        /// <summary>Represents a group that holds visual states</summary>
        var self = this;

        this.defaultDuration = Dolittle.TimeSpan.zero();

        /// <field name="currentState" type="Dolittle.interaction.VisualState">Holds the current state, this is an observable</field>
        this.currentState = ko.observable({name: "null state", enter: function () {}, exit: function () {}});

        /// <field name="states" type="Array" elementType="Dolittle.interaction.VisualState">Holds an observable array of visual states</field>
        this.states = ko.observableArray();

        /// <field name="transitions" type="Array" elementType="Dolittle.interaction.VisualStateTransition">Holds an observable array of visual state transitions</field>
        this.transitions = ko.observableArray();

        this.addState = function (state) {
            /// <summary>Add a state to the group</summary>
            /// <param name="state" type="Dolittle.interaction.VisualState">State to add</param>
            if (self.hasState(state.name)) {
                throw "VisualState with name of '" + state.name + "' already exists";
            }
            self.states.push(state);
        };

        this.addTransition = function (transition) {
            /// <summary>Add transition to group</summary>
            /// <param name="transition" type="Dolittle.interaction.VisualStateTransition">Transition to add</param>
            self.transitions.push(transition);
        };

        this.hasState = function (stateName) {
            /// <summary>Check if group has state by the name of the state</summary>
            /// <param name="stateName" type="String">Name of the state to check for</param>
            /// <returns type="Boolean">True if the state exists, false if not</returns>
            var hasState = false;
            self.states().forEach(function (state) {
                if (state.name === stateName) {
                    hasState = true;
                    return;
                }
            });

            return hasState;
        };

        this.getStateByName = function (stateName) {
            /// <summary>Gets a state by its name</summary>
            /// <param name="stateName" type="String">Name of the state to get</param>
            /// <returns type="Dolittle.interaction.VisualState">State found or null if it does not have a state by the given name</returns>
            var stateFound = null;
            self.states().forEach(function (state) {
                if (state.name === stateName) {
                    stateFound = state;
                    return;
                }
            });
            return stateFound;
        };

        this.goTo = function (namingRoot, stateName) {
            /// <summary>Go to a specific state by the name of the state</summary>
            /// <param name="stateName" type="String">Name of the state to go to</param>
            var currentState = self.currentState();
            if (!Dolittle.isNullOrUndefined(currentState) && currentState.name === stateName) {
                return;
            }

            var state = self.getStateByName(stateName);
            if (!Dolittle.isNullOrUndefined(state)) {
                var duration = self.defaultDuration;
                if (!Dolittle.isNullOrUndefined(currentState)) {
                    currentState.exit(namingRoot, duration);
                }
                state.enter(namingRoot, duration);

                dispatcher.schedule(duration.totalMilliseconds(), function () {
                    self.currentState(state);
                });
            }
        };
    })
});