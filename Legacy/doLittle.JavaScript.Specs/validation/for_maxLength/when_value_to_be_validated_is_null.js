describe("when value to be validated is null", function () {
    var validator = Dolittle.validation.maxLength.create({ options: { length: 3 } })
    var result = validator.validate(null)

    it("should be valid", function () {
        expect(result).toBe(true);
    });
});
