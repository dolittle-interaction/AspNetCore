Dolittle.namespace("Dolittle", {
    isType: function (o) {
        if (Dolittle.isNullOrUndefined(o)) {
            return false;
        }
		return typeof o._typeId !== "undefined";
	}
});