Dolittle.namespace("Dolittle", {
    isNumber: function (number) {
        if (Dolittle.isString(number)) {
            if (number.length > 1 && number[0] === '0') {
                return false;
            }
        }

        return !isNaN(parseFloat(number)) && isFinite(number);
    }
});