// Generated by CoffeeScript 1.3.3
(function() {
  var __bind = function(fn, me){ return function(){ return fn.apply(me, arguments); }; },
    __hasProp = {}.hasOwnProperty,
    __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor(); child.__super__ = parent.prototype; return child; };

  define('client_view', ['backbone', 'client_router', 'form_template_renderer'], function(Backbone, ClientRouter, FormTemplateRenderer) {
    var ClientView;
    return ClientView = (function(_super) {

      __extends(ClientView, _super);

      function ClientView() {
        this.load_json = __bind(this.load_json, this);
        return ClientView.__super__.constructor.apply(this, arguments);
      }

      ClientView.prototype.events = {
        'submit': 'submit_form'
      };

      ClientView.prototype.initialize = function() {
        this.setElement('#client-content');
        this.router = new ClientRouter();
        this.router.on('render', this.render_path, this);
        this.form_template_renderer = new FormTemplateRenderer();
        return Backbone.history.start();
      };

      ClientView.prototype.render = function() {
        this.$el.empty();
        if (this.data != null) {
          if (this.data.links != null) {
            this.render_links();
          }
          if (this.data.template != null) {
            this.render_template();
          }
        }
        return this;
      };

      ClientView.prototype.render_path = function(path) {
        return $.getJSON(path, this.load_json);
      };

      ClientView.prototype.load_json = function(data) {
        this.data = data;
        return this.render();
      };

      ClientView.prototype.render_links = function() {
        var link, _i, _len, _ref, _results;
        this.$el.append('<div id="links"><h1>Links</h1></div>');
        _ref = this.data.links;
        _results = [];
        for (_i = 0, _len = _ref.length; _i < _len; _i++) {
          link = _ref[_i];
          _results.push(this.append_link(link));
        }
        return _results;
      };

      ClientView.prototype.append_link = function(link) {
        return this.$('#links').append("<p>\n    <a href=\"#" + link.href + "\">" + link.rel + "</a>\n</p>");
      };

      ClientView.prototype.render_template = function() {
        this.$el.append('<div id="form-container"><h1>Form</h1></div>');
        console.log('rendered template', this.form_template_renderer.render(this.data));
        return this.$el.append(this.form_template_renderer.render(this.data));
      };

      ClientView.prototype.submit_form = function(event) {
        var data, url;
        event.preventDefault();
        url = _.find(this.data.links, function(item) {
          return item.rel === 'self';
        });
        data = {};
        $('input').not(':submit').each(function() {
          var $current;
          $current = $(this);
          return data[$current.attr('name')] = $current.val();
        });
        return $.post(url.href, data, function(res) {
          return console.log(res);
        });
      };

      return ClientView;

    })(Backbone.View);
  });

}).call(this);
