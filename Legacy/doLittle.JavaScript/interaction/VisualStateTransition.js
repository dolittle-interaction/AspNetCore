Dolittle.namespace("Dolittle.interaction", {
    VisualStateTransition: Dolittle.Type.extend(function() {
        /// <summary>Represents a description of transition between two named states</summary>

        /// <field name="from" type="String">Name of visual state that we are describing transitioning from</field>
        this.from = "";

        /// <field name="to" type="String">Name of visual state that we are describing transitioning to</field>
        this.to = "";

        /// <field name="duration" type="Dolittle.TimeStamp">Duration for the transition</field>
        this.duration = Dolittle.TimeStamp.zero();
    })
});