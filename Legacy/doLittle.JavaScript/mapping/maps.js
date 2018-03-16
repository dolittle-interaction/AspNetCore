Dolittle.namespace("Dolittle.mapping", {
    maps: Dolittle.Singleton(function () {
        var self = this;
        var maps = {};

        function getKeyFrom(sourceType, targetType) {
            return sourceType._typeId + " - " + targetType._typeId;
        }

        var extenders = Dolittle.mapping.Map.getExtenders();

        extenders.forEach(function (extender) {
            var map = extender.create();
            var key = getKeyFrom(map.sourceType, map.targetType);
            maps[key] = map;
        });

        this.hasMapFor = function (sourceType, targetType) {
            if (Dolittle.isNullOrUndefined(sourceType) || Dolittle.isNullOrUndefined(targetType)) {
                return false;
            }
            var key = getKeyFrom(sourceType, targetType);
            return maps.hasOwnProperty(key);
        };

        this.getMapFor = function (sourceType, targetType) {
            if (self.hasMapFor(sourceType, targetType)) {
                var key = getKeyFrom(sourceType, targetType);
                return maps[key];
            }
        };
    })
});