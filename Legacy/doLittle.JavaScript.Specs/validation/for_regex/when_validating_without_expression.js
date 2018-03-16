describe("when validating without expression", function () {
    var exception = null;
    try {
        var validator = Dolittle.validation.regex.create({ options: { } });
        validator.validate("1234");
    } catch (e) {
        exception = e;
    }

    it("should throw not a string exception", function () {
        expect(exception instanceof Dolittle.validation.MissingExpression).toBe(true);
    });
});