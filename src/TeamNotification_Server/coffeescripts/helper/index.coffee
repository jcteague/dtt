helper = require('../support/core_helper')
module.exports = (app) ->
    helper.require_all_files_in(__dirname,app)
