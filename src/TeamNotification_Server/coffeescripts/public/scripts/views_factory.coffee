define 'views_factory', ['messages_view', 'rooms_view'], (MessagesView, RoomsView) ->

    class ViewsFactory

        registry:
            messages: MessagesView
            rooms: RoomsView

        get_for: (collection_data) ->
            keys = _.keys(collection_data)
            (new view(model: collection_data) for key, view of @registry when keys.contains(key))
