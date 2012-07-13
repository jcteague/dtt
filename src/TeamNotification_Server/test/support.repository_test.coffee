expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

support_mock =
    core:
        entity_factory:
            get: sinon.stub()

q_mock =
    defer: sinon.stub()

Repository = module_loader.require('../support/repository', {
    requires:
        './core': support_mock
        'q': q_mock
})

describe 'Repository', ->

    sut = null
    entity = null

    beforeEach (done) ->
        entity = 'blah'
        sut = new Repository(entity)
        done()

    describe 'constructor', ->

        it 'should contain an entity field as the entity', (done) ->
            expect(sut.entity).to.equal entity
            done()

    describe 'get_by_id', ->

        db_entity = null
        id = null
        resolve_callback = null
        result = null
        expected_result = null

        beforeEach (done) ->
            db_entity =
                get: sinon.spy()
            support_mock.core.entity_factory.get.withArgs(sut.entity).returns(db_entity)
            deferred =
                promise: 'q-promise'
            q_mock.defer.returns deferred

            resolve_callback = 'blah func'
            id = 10
            sinon.stub(sut, 'get_on_resolve_callback').withArgs(deferred).returns(resolve_callback)
            expected_result = deferred.promise
            result = sut.get_by_id(id)
            done()

        it 'should return a promise', (done) ->
            expect(result).to.equal expected_result
            done()

        it 'should attemp to find the entity that matches the id', (done) ->
            sinon.assert.calledWith(db_entity.get, id, resolve_callback)
            done()

    describe 'find', ->

        db_entity = null
        arg1 = null
        arg2 = null
        resolve_callback = null
        expected_result = null
        result = null

        beforeEach (done) ->
            db_entity =
                find: 
                    apply: sinon.spy()
            support_mock.core.entity_factory.get.withArgs(sut.entity).returns(db_entity)
            deferred =
                promise: 'q-promise'
            q_mock.defer.returns deferred

            resolve_callback = 'blah func'
            [arg1, arg2] = ['foo', 'bar']
            sinon.stub(sut, 'get_on_resolve_callback').withArgs(deferred).returns(resolve_callback)

            expected_result = deferred.promise
            result = sut.find arg1, arg2
            done()

        it 'should return a promise', (done) ->
            expect(result).to.equal expected_result
            done()

        it 'should attemp to find the entities that match the arguments', (done) ->
            sinon.assert.calledWith(db_entity.find.apply, sut, [arg1, arg2, resolve_callback])
            done()

