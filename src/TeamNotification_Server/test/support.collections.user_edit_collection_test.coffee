expect = require('expect.js')
sinon = require('sinon')
module_loader = require('sandboxed-module')

UserEditCollection = module_loader.require('../support/collections/user_edit_collection', {})

describe 'User Edit Collection', ->

    sut = null

