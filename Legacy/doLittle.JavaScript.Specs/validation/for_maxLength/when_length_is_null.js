describe("when not specifying length", function () {
    var exception = null;
    try {
        var validator = Dolittle.validation.maxLength.create({ options: { length: null } })
        validator.validate("1234")
    } catch (e) {
        exception = e;
    }

    it("should throw max not specified exception", function () {
        expect(exception instanceof Dolittle.validation.MaxNotSpecified).toBe(true);
    });
});