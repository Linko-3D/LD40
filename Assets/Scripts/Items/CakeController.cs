public class CakeController : ItemController {
    
    public override void OnConsumedBy(PrincessCakeController controller) {
        base.OnConsumedBy(controller);

        controller.Model.EatCake();
    }

}
