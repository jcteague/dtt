var helper = require('../support/core_helper');
module.exports = function(app, io){
    helper.require_all_files_in(__dirname,app,io)
};
