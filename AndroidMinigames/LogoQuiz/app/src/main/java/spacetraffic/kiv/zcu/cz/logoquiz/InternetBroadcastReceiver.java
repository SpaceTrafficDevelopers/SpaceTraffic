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

import android.app.Activity;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.net.ConnectivityManager;
import android.support.v7.app.AlertDialog;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

/**
 * Internet broadcast reciever class.
 */
public class InternetBroadcastReceiver extends BroadcastReceiver {

    /**
     * Alert dialog.
     */
    private AlertDialog dialog;

    /**
     * Activity.
     */
    private Activity activity;

    /**
     * IBR constructor.
     * @param activity
     */
    public InternetBroadcastReceiver(Activity activity){
        this.activity = activity;
    }

    /**
     * Overriden onReceive method.
     * @param context context
     * @param intent intent
     */
    @Override
    public void onReceive(final Context context, Intent intent) {
        if (!isOnline()){
            dialog.show();

            if(activity instanceof GameActivity)
                ((GameActivity)activity).pauseWithRequester();

            //this is because in dialog declaration is cannot possible override close action on click on positive button
            Button theButton = dialog.getButton(DialogInterface.BUTTON_POSITIVE);
            theButton.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    if(isOnline()) {
                        dialog.dismiss();
                        if(activity instanceof GameActivity)
                            ((GameActivity)activity).resumeWithRequester();
                    }
                    else
                        Toast.makeText(context.getApplicationContext(), R.string.connectivity_dialog_toast, Toast.LENGTH_SHORT).show();
                }
            });
        }
    }

    /**
     * Method for check if mobile is online.
     * @return true if mobile is online
     */
    private boolean isOnline(){
        ConnectivityManager cm =
                (ConnectivityManager) activity.getSystemService(Context.CONNECTIVITY_SERVICE);

        return cm.getActiveNetworkInfo() != null &&
                cm.getActiveNetworkInfo().isConnectedOrConnecting();
    }

    /**
     * Method for preparing alert dialog.
     */
    public void prepareDialog()
    {
        AlertDialog.Builder builder = new AlertDialog.Builder(activity);
        builder.setMessage(R.string.connectivity_dialog_text)
                .setCancelable(false)
                .setTitle(R.string.connectivity_dialog_title)
                .setNegativeButton(R.string.finish_button,
                        new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int id) {
                                dialog.dismiss();

                                if(activity instanceof GameActivity) {
                                    GameActivity act = (GameActivity) activity;
                                    act.pauseWithRequester();
                                    act.unregisterReceiver();
                                }
                                else
                                    activity.unregisterReceiver(InternetBroadcastReceiver.this);

                                activity.finish();
                            }
                        })
                .setPositiveButton(R.string.continue_button, null);
        dialog = builder.create();
    }
}
