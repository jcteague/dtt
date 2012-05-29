var helper = require('core_helper');
module.exports = function(app){
    helper.require_all_files_in(__dirname,app)
};

