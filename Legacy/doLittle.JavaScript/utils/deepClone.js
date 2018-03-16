Dolittle.namespace("Dolittle",{
    deepClone: function (source, target) {
        function isReservedMemberName(member) {
            return member.indexOf("_") >= 0 || member === "model" || member === "commons" || member === "targetViewModel" || member === "region";
        }

        if (ko.isObservable(source)) {
            source = source();
        }

        if (target == null) {
            if (Dolittle.isArray(source)) {
                target = [];
            } else {
                target = {};
            }
        }

        var sourceValue;
        if (Dolittle.isArray(source)) {
            for (var index = 0; index < source.length; index++) {
                sourceValue = source[index];
                var clonedValue = Dolittle.deepClone(sourceValue);
                target.push(clonedValue);
            }
        } else {
            for (var member in source) {
                if (isReservedMemberName(member)) {
                    continue;
                }

                sourceValue = source[member];

                if (ko.isObservable(sourceValue)) {
                    sourceValue = sourceValue();
                }

                if (Dolittle.isFunction(sourceValue)) {
                    continue;
                }

                var targetValue = null;
                if (Dolittle.isObject(sourceValue)) {
                    targetValue = {};
                } else if (Dolittle.isArray(sourceValue)) {
                    targetValue = [];
                } else {
                    target[member] = sourceValue;
                }

                if (targetValue != null) {
                    target[member] = targetValue;
                    Dolittle.deepClone(sourceValue, targetValue);
                }
            }
        }

        return target;
    }
});
