// Generated by CoffeeScript 1.3.3
(function() {
  var CollectionActionResolver, Q, Repository, add_user_to_chat_room, build;

  Q = require('q');

  Repository = require('./repository');

  CollectionActionResolver = require('./collection_action_resolver');

  build = function(collection_type) {
    return new CollectionActionResolver(collection_type);
  };

  add_user_to_chat_room = function(user_id, room_id) {
    var chat_room_repository, defer, user_repository;
    user_repository = new Repository('User');
    chat_room_repository = new Repository('ChatRoom');
    defer = Q.defer();
    user_repository.get_by_id(user_id).then(function(user) {
      if (user != null) {
        return chat_room_repository.get_by_id(room_id).then(function(chat_room) {
          var member;
          if (((function() {
            var _i, _len, _ref, _results;
            _ref = chat_room.users;
            _results = [];
            for (_i = 0, _len = _ref.length; _i < _len; _i++) {
              member = _ref[_i];
              if (member.id === user_id) {
                _results.push(member);
              }
            }
            return _results;
          })()).length === 0) {
            return chat_room.addUsers(user, function() {
              return defer.resolve({
                success: true,
                messages: ["user " + user.id + " added"]
              });
            });
          } else {
            return defer.resolve({
              success: false,
              messages: ["user " + user_id + " is already in the room"]
            });
          }
        });
      } else {
        return defer.resolve({
          success: false,
          messages: ["user " + user_id + " does not exist"]
        });
      }
    });
    return defer.promise;
  };

  module.exports = {
    build: build,
    add_user_to_chat_room: add_user_to_chat_room
  };

}).call(this);
