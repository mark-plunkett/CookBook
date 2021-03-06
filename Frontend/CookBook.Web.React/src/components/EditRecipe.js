import React, { Component } from 'react';
import { Button, Container, Heading } from 'react-bulma-components';
import { updateRecipe, recipeStore } from '../models/recipes';
import { InstructionsInput } from './RecipeForm/InstructionsInput';
import { IngredientsInput } from './RecipeForm/IngredientsInput';
import { TitleInput } from './RecipeForm/TitleInput';
import { DescriptionInput } from './RecipeForm/DescriptionInput';
import { NumberOfServingsInput } from './RecipeForm/NumberOfServingsInput';

import { Form as FinalForm } from 'react-final-form';
import { mapErrorsToObject } from 'services/businessError';
import { TagsInput } from './RecipeForm/TagsInput';
import { tagStore } from 'models/tags';

export class EditRecipe extends Component {

    constructor(props) {
        super(props);
        this.id = props.match.params.id;
        this.state = {
            recipeTags: [],
            suggestedTags: []
        };
        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.setTags = this.setTags.bind(this);
    }

    async componentDidMount() {
        let allTags = await tagStore.list();
        allTags = allTags.map(tag => { return { id: tag.canonicalized, name: tag.name }});
        let newState = await recipeStore.get(this.id);
        let recipeTags = allTags.filter(tag => newState.tags.includes(tag.id));
        this.setState({
            ...newState,
            recipeTags: recipeTags,
            suggestedTags: allTags
        });
    }

    handleChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;
        this.setState({
            [name]: value
        });
    }

    setTags(tags) {
        this.setState({
            ...this.state,
            tags: tags.map(t => t.name),
            recipeTags: tags
        });
    }

    async handleSubmit() {
        try {
            await updateRecipe(this.state);
            this.props.history.push("/");
        }
        catch (e) {
            if (e.response.status === 400)
                return mapErrorsToObject(e.response.data.businessErrors);

            throw e;
        }
    }

    render() {
        return (
            <Container>
                <Heading size={3}>Edit Recipe '{this.state.title}'</Heading>
                <FinalForm
                    onSubmit={this.handleSubmit}
                    render={({ handleSubmit }) => (
                        <form onSubmit={handleSubmit}>
                            <TitleInput title={this.state.title} handleChange={this.handleChange} />
                            <NumberOfServingsInput servings={this.state.servings} handleChange={this.handleChange} />
                            <TagsInput tags={this.state.recipeTags} suggestions={this.state.suggestedTags} handleChange={this.setTags} />
                            <DescriptionInput description={this.state.description} handleChange={this.handleChange} />
                            <InstructionsInput instructions={this.state.instructions} handleChange={this.handleChange} />
                            <IngredientsInput ingredients={this.state.ingredients} handleChange={this.handleChange} />
                            <Button className="is-success" type="submit">Update</Button>
                        </form>
                    )} />
            </Container>
        );
    }
}