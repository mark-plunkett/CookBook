import React, { Component } from 'react';
import { Form } from 'react-bulma-components';
import ReactTags from 'react-tag-autocomplete';

const { Field, Control, Label } = Form;

export class TagsInput extends Component {
    constructor(props) {
        super(props);

        this.state = {
            tags: props.tags,
            suggestions: props.suggestions
        };
        this.reactTags = React.createRef();
    }

    static getDerivedStateFromProps(nextProps, prevState) {
        let uniqueTags = new Set(prevState.tags.concat(nextProps.tags));
        return {
            tags: [...uniqueTags],
            suggestions: nextProps.suggestions.length > 0 ? nextProps.suggestions : prevState.suggestions
        };
    }

    componentDidUpdate(prevProps, prevState) {
        if (this.state.tags.length === prevState.tags.length
            && this.state.suggestions.length === prevState.suggestions.length)
            return;

        this.setState({
            suggestions: prevProps.suggestions,
            tags: prevProps.tags
        })
    }

    onAddition(tag) {
        const tags = [].concat(this.state.tags, tag);
        this.setState({
            ...this.state,
            tags: tags
        });
        this.props.handleChange(tags);
    }

    onDelete(i) {
        const tags = this.state.tags.slice(0);
        tags.splice(i, 1);
        this.setState({
            ...this.state,
            tags: tags
        });
        this.props.handleChange(tags);
    }

    render() {
        return (
            <Field>
                <Control>
                    <Label>Tags</Label>
                    <ReactTags
                        ref={this.reactTags}
                        tags={this.state.tags}
                        suggestions={this.state.suggestions}
                        onAddition={this.onAddition.bind(this)}
                        onDelete={this.onDelete.bind(this)}
                        allowBackspace={false}
                        allowNew={true}
                    />
                </Control>
            </Field>
        )
    }
}
