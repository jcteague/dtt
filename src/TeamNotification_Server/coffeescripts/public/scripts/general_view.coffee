define 'general_view', ['backbone'], (Backbone) ->

    class GeneralView extends Backbone.View
        id: 'view_id'

        events: () ->
            return

        initialize: ->
            return

        render: ->
            return

        listen_to:(param) ->
            return

        append_to: (parent) ->
            @$el.appendTo parent
            
        propagate_event: (event, values) ->
            @trigger event, values
            
        get_model_for:(attribute_name)->
            obj =
                get: @model.get
                has: @model.has
                on: @model.on
                attributes: @model.get(attribute_name)
            return obj
            
        get_link: (rel, links_collection)->
            if links_collection?
                for link in links_collection
                    if(link.rel == rel)
                        return """<a href="##{link.href}">#{link.name}</a>"""
            return ""

        get_field: (field, collection)->
            if collection?
                for obj in collection
                    if(obj.name == field)
                        return obj.value
            return ""
