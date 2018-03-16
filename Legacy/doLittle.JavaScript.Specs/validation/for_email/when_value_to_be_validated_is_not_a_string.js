describe("when the value to be validated is not a string", function () {
    var exception = null;
    try {
        var validator = Dolittle.validation.email.create({ options: {} });
        validator.validate({});
    } catch (e) {
        exception = e;
    }

    it("should throw an exception", function () {
        expect(exception instanceof Dolittle.validation.NotAString).toBe(true);
    });
});