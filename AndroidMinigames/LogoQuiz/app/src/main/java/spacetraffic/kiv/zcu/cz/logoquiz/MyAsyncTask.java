/**
 Copyright 2010 FAV ZCU

 Licensed under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License.
 You may obtain a copy of the License at

 http://www.apache.org/licenses/LICENSE-2.0

 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.

 **/
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
