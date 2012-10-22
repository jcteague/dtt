expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

support_mock =
    core:
        entity_factory:
            get: sinon.stub()
            create: sinon.stub()

q_mock =
    defer: sinon.stub()

Repository = module_loader.require('../support/repository', {
    requires:
        './core': support_mock
        'q': q_mock
})

describe 'Repository', ->

    sut = entity = null

    beforeEach (done) ->
        entity = 'blah'
        sut = new Repository(entity)
        done()

    describe 'constructor', ->

        it 'should contain an entity field as the entity', (done) ->
            expect(sut.entity).to.equal entity
            done()

    describe 'get_by_id', ->

        db_entity = id = resolve_callback = result = expected_result = null

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

        db_entity = arg1 = arg2 = resolve_callback = expected_result = result = null

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

    describe 'save', ->

        result = expected_result = entity_data = saved_entity = deferred = null

        beforeEach (done) ->
            entity_data =
                name: 'foo'
                email: 'bar'

            saved_entity =
                id: 99
                name: 'foo'
            db_entity =
                save: (callback) ->
                    callback(false, saved_entity)
            support_mock.core.entity_factory.create.withArgs(sut.entity, entity_data).returns(db_entity)

            deferred =
                promise: 'q-promise'
                resolve: sinon.stub()
            q_mock.defer.returns deferred

            expected_result = deferred.promise
            result = sut.save entity_data
            done()

        it 'should return a promise', (done) ->
            expect(result).to.equal expected_result
            done()

        it 'should attemp to create the entity with the arguments', (done) ->
            sinon.assert.calledWith(support_mock.core.entity_factory.create, sut.entity, entity_data)
            done()

        it 'should resolve the promise with the saved entity', (done) ->
            sinon.assert.calledWith(deferred.resolve, saved_entity)
            done()

    describe 'update', ->

        result = expected_result = entity_data = deferred = updated_entity = db_entity = save_spy = null

        beforeEach (done) ->
            entity_data =
                id: 99
                name: 'foo updated'
                email: 'bar updated'

            save_spy = sinon.spy()

            db_entity =
                id: 99
                name: 'foo'
                email: 'bar'
                save: (callback) ->
                    save_spy()
                    callback(false, updated_entity)

            db_entity_promise =
                then: (callback) ->
                    callback(db_entity)
            sinon.stub(sut, 'get_by_id').withArgs(entity_data.id).returns(db_entity_promise)
            deferred =
                promise: 'q-promise'
                resolve: sinon.stub()
            q_mock.defer.returns deferred

            expected_result = deferred.promise
            result = sut.update(entity_data)
            done()

        it 'should return a promise', (done) ->
            expect(result).to.equal expected_result
            done()

        it 'should save the entity', (done) ->
            sinon.assert.called(save_spy)
            done()

        it 'should resolve the promise with the saved entity', (done) ->
            sinon.assert.calledWith(deferred.resolve, updated_entity)
            done()
