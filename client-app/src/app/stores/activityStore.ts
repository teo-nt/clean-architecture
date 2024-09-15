import { action, makeAutoObservable, makeObservable, observable, runInAction } from "mobx"
import { Activity } from "../models/activity"
import agent from "../api/agent";
import {v4 as uuid } from 'uuid'

export default class ActivityStore {
    // activities: Activity[] = []
    activityRegistry = new Map<string, Activity>()
    selectedActivity: Activity | undefined = undefined;
    editMode = false;
    loading = false
    loadingInitial = false

    constructor() {
        /*makeObservable(this, {
            title: observable,
            setTitle: action
        })*/

        makeAutoObservable(this)
    }

    get activitiesByDate() {
        return Array.from(this.activityRegistry.values()).sort((a, b) => Date.parse(a.date) - Date.parse(b.date))
    }

    loadActivities = async () => {
        this.loadingInitial = true
        try {
            const activities = await agent.Activities.list()
            runInAction(() => {
                activities.forEach(activity => {
                    this.setActivity(activity)
                })
                this.loadingInitial = false
            })
           
        } catch (error) {
            console.log(error)
            runInAction(() => {
                this.loadingInitial = false
            })           
        }
    }

    loadActivity = async (id: string) => {
        let activity = this.getActivity(id)
        if (activity) {
            this.selectedActivity = activity
            return activity
        } 
        else {
            this.loadingInitial = true
            try {
                const activityReturned = await agent.Activities.details(id)              
                runInAction(() => {
                    this.setActivity(activityReturned)
                    this.selectedActivity = activityReturned
                    this.loadingInitial = false                   
                })
                return activityReturned
            } catch (error) {
                console.log(error)
                this.loadingInitial = false
            }
        }
    }

    private setActivity = (activity: Activity) => {
        activity.date = activity.date.split('T')[0]
        this.activityRegistry.set(activity.id, activity)
    }

    private getActivity = (id: string) => {
        return this.activityRegistry.get(id)
    }

    createActivity = async (activity: Activity) => {
        this.loading = true
        activity.id = uuid()
        try {
            await agent.Activities.create(activity)
            runInAction(() => {
                //this.activities.push(activity)
                this.activityRegistry.set(activity.id, activity)
                this.selectedActivity = activity
                this.editMode = false
                this.loading = false
            })
        } catch (error) {
            console.log(error)
            runInAction(() => {
                this.loading = false
            })
        }
    }

    updateActivity = async (activity: Activity) => {
        this.loading = true
        try {
            await agent.Activities.update(activity)
            runInAction(() => {
                //this.activities = [...this.activities.filter(a => a.id !== activity.id), activity]
                this.activityRegistry.set(activity.id, activity)
                this.selectedActivity = activity
                this.editMode = false;
                this.loading = false
            })
        } catch (error) {
            console.log(error)
            runInAction(() => {
                this.loading = false
            })
        }
    }

    deleteActivity = async (id: string) => {
        this.loading = true
        try {
            await agent.Activities.delete(id)
            runInAction(() => {
                //this.activities = [...this.activities.filter(a => a.id !== id)]
                this.activityRegistry.delete(id)
                this.loading = false
            })
        } catch (error) {
            console.log(error)
            runInAction(() => {
                this.loading = false
            })
        }
    }
}