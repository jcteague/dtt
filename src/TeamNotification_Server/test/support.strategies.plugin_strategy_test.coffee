should = require('should')
sinon = require('sinon')
expect = require('expect.js')
module_loader = require('sandboxed-module')
config = require('../config')()

file_reader_mock =
    read_as_json: sinon.stub()

sut = module_loader.require('../support/strategies/plugin_strategy', {
    requires:
        '../system/file_reader': file_reader_mock
})

describe 'Plugin Version Strategy', ->

    describe 'strategy', ->

        result = expected_result = null

        beforeEach (done) ->
            expected_result = 'blah result'
            file_reader_mock.read_as_json.withArgs(config.plugins.visual_studio.manifest).returns(expected_result)
            result = sut()
            done()

        it 'should return the file reader result', (done) ->
            expect(result).to.equal expected_result
            done()
