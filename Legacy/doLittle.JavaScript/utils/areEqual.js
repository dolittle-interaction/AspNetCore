Dolittle.namespace("Dolittle",{
    areEqual: function (source, target) {
        function isReservedMemberName(member) {
            return member.indexOf("_") >= 0 || member === "model" || member === "commons" || member === "targetViewModel" || member === "region";
        }

        if (ko.isObservable(source)) {
            source = source();
        }
        if (ko.isObservable(target)) {
            target = target();
        }

        if (Dolittle.isNullOrUndefined(source) && Dolittle.isNullOrUndefined(target)) {
            return true;
        }

        if (Dolittle.isNullOrUndefined(source)) {
            return false;
        }
        if (Dolittle.isNullOrUndefined(target)) {
            return false;
        }

        if (Dolittle.isArray(source) && Dolittle.isArray(target)) {
            if (source.length !== target.length) {
                return false;
            }

            for (var index = 0; index < source.length; index++) {
                if (Dolittle.areEqual(source[index], target[index]) === false) {
                    return false;
                }
            }
        } else {
            for (var member in source) {
                if (isReservedMemberName(member)) {
                    continue;
                }
                if (target.hasOwnProperty(member)) {
                    var sourceValue = source[member];
                    var targetValue = target[member];

                    if (Dolittle.isObject(sourceValue) ||
                        Dolittle.isArray(sourceValue) ||
                        ko.isObservable(sourceValue)) {

                        if (!Dolittle.areEqual(sourceValue, targetValue)) {
                            return false;
                        }
                    } else {
                        if (sourceValue !== targetValue) {
                            return false;
                        }
                    }
                } else {
                    return false;
                }
            }
        }

        return true;
    }
});