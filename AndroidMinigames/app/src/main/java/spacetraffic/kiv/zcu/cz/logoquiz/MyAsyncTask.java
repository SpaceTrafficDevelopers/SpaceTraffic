package spacetraffic.kiv.zcu.cz.logoquiz;

import android.os.AsyncTask;

/**
 * Class as asynchronous task with callback.
 */
public class MyAsyncTask extends AsyncTask<Sender, Void, Object> {

    /**
     * Object with callback.
     */
    private IPostAction postAction;

    /**
     * MyAsyncTask constructor.
     * @param postAction callback
     */
    public MyAsyncTask(IPostAction postAction)
    {
        this.postAction = postAction;
    }

    /**
     * Do in background method
     * @param params sender params
     * @return result
     */
    @Override
    protected Object doInBackground(Sender... params) {
        return params[0].callMethod();
    }

    /**
     * On post execute method. Calls the callback.
     * @param reuslt result
     */
    @Override
    protected void onPostExecute(Object reuslt){
        this.postAction.doAction(reuslt);
    }
}
