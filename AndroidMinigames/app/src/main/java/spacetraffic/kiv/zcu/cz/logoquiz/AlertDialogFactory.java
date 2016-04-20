package spacetraffic.kiv.zcu.cz.logoquiz;

import android.app.Activity;
import android.content.DialogInterface;
import android.content.res.Resources;
import android.support.v7.app.AlertDialog;

/**
 * Alert dialog factory class. This is not classic factory class with static factory methods.
 */
public class AlertDialogFactory {

    /**
     * Activity.
     */
    private Activity activity;

    /**
     * Resources.
     */
    private Resources resources;

    /**
     * Alert dialog factory constructor.
     * @param activity activity
     */
    public AlertDialogFactory(Activity activity){
        this.activity = activity;
        this.resources = activity.getResources();
    }

    /**
     * Method for showing info dialog.
     */
    public void showInfoDialog(){
        AlertDialog.Builder builder = new AlertDialog.Builder(activity);
        builder.setMessage(R.string.info_dialog_text)
                .setCancelable(false)
                .setTitle(R.string.info_dialog_title)
                .setPositiveButton(R.string.start_game,
                        new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int id) {
                                dialog.dismiss();
                                ((GameActivity)activity).startTimer();
                            }
                        });
        builder.create().show();
    }

    /**
     * Method for showing finish confirm dialog.
     */
    public void showFinishConfirmDialog(){
        AlertDialog.Builder builder = new AlertDialog.Builder(activity);
        builder.setMessage(R.string.confirm_dialog_text)
                .setCancelable(false)
                .setTitle(R.string.confirm_dialog_title)
                .setPositiveButton(R.string.continue_button,
                        new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int id) {
                                dialog.dismiss();
                                ((GameActivity)activity).resumeTimer();
                            }
                        })
                .setNegativeButton(R.string.finish_button,
                        new DialogInterface.OnClickListener(){
                            @Override
                            public void onClick(DialogInterface dialog, int which) {
                                dialog.dismiss();
                                ((GameActivity)activity).prepareFinish();
                            }
                        });
        builder.create().show();
    }

    /**
     * Method for showing unexpected error dialog.
     */
    public void showUnexpectedErrorDialog(){
        Resources res = activity.getResources();
        showFinishDialog(res.getString(R.string.unexpected_error_title), res.getString(R.string.unexpected_error));
    }

    /**
     * Method for showing win dialog.
     */
    public void showWinDialog(int numberOfCorrectAnswers, int numberOfAnswers){
        showFinishDialog(this.resources.getString(R.string.game_dialog_win_title),
                String.format(this.resources.getString(R.string.game_dialog_win_text), numberOfCorrectAnswers, numberOfAnswers));
    }

    /**
     * Method for showing loose dialog.
     */
    public void showLooseDialog(int numberOfCorrectAnswers, int numberOfAnswers){
        showFinishDialog(this.resources.getString(R.string.game_dialog_loose_title),
                String.format(this.resources.getString(R.string.game_dialog_loose_text), numberOfCorrectAnswers, numberOfAnswers));
    }

    /**
     * Method for showing not alive dialog.
     */
    public void showNotAliveDialog(){
        showFinishDialog(this.resources.getString(R.string.not_alive_title), this.resources.getString(R.string.not_alive_text));
    }

    /**
     * Method for showing finish dialog.
     */
    public void showFinishDialog(String title, String text){

        AlertDialog.Builder builder = new AlertDialog.Builder(activity);
        builder.setMessage(text)
                .setCancelable(false)
                .setTitle(title)
                .setPositiveButton(R.string.game_dialog_finish_button,
                        new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int id) {
                                dialog.dismiss();
                                activity.finish();
                            }
                        });
        builder.create().show();
    }
}
