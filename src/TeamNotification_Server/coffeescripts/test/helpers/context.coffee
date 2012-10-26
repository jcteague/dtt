unit_test = (path, mocks) ->
    sut = ''

    return (block) ->

        describe 'This is a unit test context', ->
            block(sut, dependencies)

integration_test = (block) ->

    describe 'This is an integration test context', ->

        block()


methods.export =
    for:
        unit_test: unit_test
        integration_test: integration_test
