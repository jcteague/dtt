// Generated by CoffeeScript 1.3.3
(function() {
  var CollectionFactory, mapping;

  mapping = {
    'user_collection': require('./collections/user_collection')
  };

  CollectionFactory = (function() {

    function CollectionFactory(type) {
      this.type = type;
    }

    CollectionFactory.prototype["for"] = function(options) {
      return new mapping[this.type](options);
    };

    return CollectionFactory;

  })();

  module.exports = CollectionFactory;

}).call(this);
