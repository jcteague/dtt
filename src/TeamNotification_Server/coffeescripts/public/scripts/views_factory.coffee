define 'views_factory', ['messages_view', 'links_view', 'rooms_view', 'form_view', 'query_view', 'chat_room_view', 'login_view'], (MessagesView, LinksView, RoomsView, FormView, QueryView, ChatRoomView, LoginView) ->
    class ViewsFactory
        registry:
            messages: MessagesView
            rooms: RoomsView
            template: FormView
            queries: QueryView
            links: LinksView
        pages:
            login: {pattern:/user\/login/g, view: LoginView}
            chat_room: { pattern:/room\/\d+\/messages/g, view: ChatRoomView }
        get_for: (collection_model) ->
            collection_data = collection_model.attributes
            for page, props of @pages
                if props.pattern.test(collection_data.href) or props.pattern.test(collection_data.href)
                    return [(new props.view(model:collection_model))] 
            
            keys = _.keys(collection_data)
            (new view(model: collection_model) for key, view of @registry when keys.contains(key))
