describe("when validating with null", function () {
    var validator = Dolittle.validation.notNull.create({ options: {} });
    var result = validator.validate(null);

    it("should not be valid", function () {
        expect(result).toBe(false);
    });
});