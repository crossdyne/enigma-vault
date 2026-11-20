export class DateHelper {
    static difference(date: Date | null) : number | null {
        if(!date)
            return null;

        return Math.max(0, Math.ceil((new Date(date).getTime() - Date.now()) / (1000 * 60 * 60 * 24)));
    }
}